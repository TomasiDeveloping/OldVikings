import {Component, inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {FeedbackModel} from "../../models/feedback.model";
import {FeedbackService} from "../../services/feedback.service";
import {FeedbackStatus} from "../../helpers/feedbackStatus";
import {Router} from "@angular/router";
import {CreateFeedbackModel} from "../../models/createFeedback.model";
import {ToastrService} from "ngx-toastr";
import {TranslateService} from "@ngx-translate/core";

@Component({
  selector: 'app-feedback',
  templateUrl: './feedback.component.html',
  styleUrl: './feedback.component.scss'
})
export class FeedbackComponent implements OnInit {
  form!: FormGroup;

  items: FeedbackModel[] = [];
  loading = false;

  private readonly fb: FormBuilder = inject(FormBuilder);
  private readonly _feedbackService: FeedbackService = inject(FeedbackService);
  private readonly _router: Router = inject(Router);
  private readonly _toaster: ToastrService = inject(ToastrService);
  private readonly _translate: TranslateService = inject(TranslateService);

  ngOnInit() {
    this.createForm();
    const status: FeedbackStatus[] = ['New', 'UnderReview'];
    this.loadWithStatuses(status)
  }

  createForm() {
    this.form = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(100)]],
      category: [1, Validators.required],
      message: ['', [Validators.required, Validators.maxLength(4000)]],
      visibility: [1, Validators.required],
      displayName: [''],
    });
  }


  openDetail(item: FeedbackModel) {
    this._router.navigate(['feedback', item.id]).then();
  }


  statusFilterMap = {
    New: true,
    UnderReview: true,
    Planned: false,
    Implemented: false,
    Rejected: false,
    Archived: false,
  };

  toggleStatus(status: FeedbackStatus) {
    // Toggle
    this.statusFilterMap[status] = !this.statusFilterMap[status];
    this.onStatusFilterChanged();
  }

  onStatusFilterChanged() {
    const selectedStatuses = Object
      .entries(this.statusFilterMap)
      .filter(([_, checked]) => checked)
      .map(([status]) => status as FeedbackStatus);

    // Falls gar nichts gewählt ist, kannst du entweder:
    // a) alles anzeigen, oder
    // b) nichts anzeigen – hier Beispiel a):
    if (selectedStatuses.length === 0) {
      this.items = [];
      return;
    }

    this.loadWithStatuses(selectedStatuses);
  }

  loadWithStatuses(statuses: FeedbackStatus[]) {
    this.loading = true;
    this._feedbackService.getFeedbacksByStatus(statuses).subscribe({
      next: res => {
        this.items = res;
        this.loading = false;
      },
      error: () => { this.loading = false; }
    });
  }

  hasAnyStatusSelected(): boolean {
    return Object.values(this.statusFilterMap).some(v => v);
  }

  categoryKey(category: number): string {
    switch (category) {
      case 0: return 'Feedback.Category.Improvement';
      case 1: return 'Feedback.Category.Suggestion';
      case 2: return 'Feedback.Category.Wish';
      case 3: return 'Feedback.Category.EventIdea';
      case 4: return 'Feedback.Category.RulesProcess';
      case 5: return 'Feedback.Category.CommunicationTools';
      case 6: return 'Feedback.Category.Complaint';
      case 7: return 'Feedback.Category.Reclamation';
      case 8: return 'Feedback.Category.Conflict';
      case 9: return 'Feedback.Category.LeadershipFeedback';
      case 99: return 'Feedback.Category.Other';
      default: return 'Feedback.Category.Other';
    }
  }



  onSubmit() {
    if (this.form.invalid) return;


    const newFeedback: CreateFeedbackModel = this.form.value as CreateFeedbackModel;
    this._feedbackService.createFeedback(newFeedback).subscribe({
      next: (res) => {
        if (res) {
          const status: FeedbackStatus[] = ['New', 'UnderReview'];
          this.loadWithStatuses(status);
          this.form.reset();
          this.form.patchValue({title: '', category: 1, message: '', visibility: 1, displayName: ''});
          this._toaster.success(this._translate.instant('Feedback.Form.Success'));
        } else {
          this._toaster.error(this._translate.instant('Feedback.Form.Error'));
        }
      },error: (err) => {
        console.log(err);
        this._toaster.error(this._translate.instant('Feedback.Form.Error'));
      }
    });


1  }

}
