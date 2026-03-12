import {Component, inject, OnInit} from '@angular/core';
import {FeedbackModel} from "../../../models/feedback.model";
import {FeedbackHistoryModel} from "../../../models/feedbackHistory.model";
import {ActivatedRoute, Router} from "@angular/router";
import {FeedbackService} from "../../../services/feedback.service";
import { FeedbackStatusCode} from "../../../helpers/feedbackStatus";
import {DeeplTranslateService} from "../../../services/deeplTranslate.service";
import {TranslationRequestModel} from "../../../models/translationRequest.model";

@Component({
  selector: 'app-feedback-detail',
  templateUrl: './feedback-detail.component.html',
  styleUrl: './feedback-detail.component.scss'
})
export class FeedbackDetailComponent implements OnInit{

  item: FeedbackModel | null = null;
  history: FeedbackHistoryModel[] = [];
  loading = false;
  historyLoading = false;
  voting = false;
  translatedMessage: string = '';
  browserLang: string = 'en';
  loadingTranslation: boolean = false;
  translationError: boolean = false;


  private readonly _route: ActivatedRoute = inject(ActivatedRoute);
  private readonly _router: Router = inject(Router);
  private readonly _feedbackService: FeedbackService = inject(FeedbackService);
  private readonly _deeplService: DeeplTranslateService = inject(DeeplTranslateService);

  private readonly _votedKey = 'ov_feedback_voted_ids';

  ngOnInit() {
    const id = this._route.snapshot.paramMap.get('id');
    if (!id) {
      this._router.navigate(['feedback']).then();
      return;
    }

    this.browserLang = navigator.language.split('-')[0];

    this.load(id);
  }

  private load(id: string) {
    this.loading = true;

    this._feedbackService.getFeedback(id).subscribe({
      next: (item) => {
        this.item = item;
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        console.log(err);
      }
    })

    this.historyLoading = true;
    this._feedbackService.getHistory(id).subscribe({
      next: (history) => {
        this.history = history;
        this.historyLoading = false;
      }, error: (err) => {
        this.historyLoading = false;
        console.log(err);
      }
    })
  }

  translateMessage() {
    if (!this.item?.message) return;

    this.loadingTranslation = true;
    const request: TranslationRequestModel = {
      language: this.browserLang,
      text: this.item.message
    };
    this._deeplService.translateText(request).subscribe({
      next: (result) => {
        if (result) {
          this.translatedMessage = this.makeLinksClickable(result.translatedText);
          this.loadingTranslation = false;
          this.translationError = false;
        } else {
          this.loadingTranslation = false;
          this.translatedMessage = '';
          this.translationError = true;
        }
      }, error: (err) => {
        this.loadingTranslation = false;
        this.translatedMessage = '';
        this.translationError = true;
        console.log(err);
      }
    })
  }

  back() {
    this._router.navigate(['feedback']).then();
  }

  makeLinksClickable(text: string): string {
    if (!text) return '';

    const replaced = text.replace(
      /(https?:\/\/[^\s<]+)/g,
      function(url) {
        return `<a href="${url}" target="_blank" rel="noopener noreferrer">${url}</a>`;
      }
    );

    return replaced.replace(/\n/g, '<br>');
  }

  canVote(item: FeedbackModel | null): boolean {
    if (!item) return false;

    const isOpen = (
      item.status === FeedbackStatusCode.New ||
        item.status === FeedbackStatusCode.UnderReview
    );

    if (!isOpen) return false;

    return !this.hasVoted(item.id);
  }

  vote(): void {
    if (!this.item || !this.canVote(this.item)) {
      return;
    }
    this.voting = true;
    const id = this.item.id;

    this._feedbackService.vote(id).subscribe({
      next: (success) => {
        if (success) {
          this.markVoted(id);
          this.voting = false;
          this.load(id);
        } else {
          this.voting = false;
        }
      }, error: (err) => {
        this.voting = false;
        console.log(err);
      }
    })
  }

  statusKeyFromCode(code: number): string {
    return `Feedback.Status.${code.toString()}`;
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
      case 10: return 'Feedback.Category.Other';
      default: return 'Feedback.Category.Other';
    }
  }

  alreadyVoted(): boolean {
    return this.item ? this.hasVoted(this.item.id) : false;
  }

  private getVotedIds(): string[] {
    if (typeof sessionStorage === 'undefined') return [];
    const raw = sessionStorage.getItem(this._votedKey);
    if (!raw) return [];
    try {
      const arr: any = JSON.parse(raw);
      return Array.isArray(arr) ? arr : [];
    } catch {
      return [];
    }
  }

  private setVotedIds(ids: string[]): void {
    if (typeof sessionStorage === 'undefined') return;
    sessionStorage.setItem(this._votedKey, JSON.stringify(ids));
  }

  private hasVoted(feedbackId: string): boolean {
    return this.getVotedIds().includes(feedbackId);
  }

  private markVoted(feedbackId: string): void {
    const current = this.getVotedIds();
    if (!current.includes(feedbackId)) {
      current.push(feedbackId);
      this.setVotedIds(current);
    }
  }

  protected readonly FeedbackStatusCode = FeedbackStatusCode;
}
