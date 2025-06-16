import {Component, inject, OnInit} from '@angular/core';
import {R4RoleModel} from "../../models/r4Role.model";
import {R4RoleService} from "../../services/r4-role.service";

@Component({
  selector: 'app-r4-roles',
  templateUrl: './r4-roles.component.html',
  styleUrl: './r4-roles.component.scss'
})
export class R4RolesComponent implements OnInit {

  private readonly _r4RoleService: R4RoleService = inject(R4RoleService);

  r4Roles: R4RoleModel[] = [];
  leader: R4RoleModel | undefined;

  ngOnInit() {
    this.r4Roles = [];
    this.leader = undefined;
    this._r4RoleService.getR4Roles().subscribe({
      next: (response: R4RoleModel[]) => {
        if (response) {
          response.forEach(role => {
            if (role.roleName === 'Alliance Marshal') {
              this.leader = role;
            } else {
              this.r4Roles.push(role);
            }
          })
        }
      }
    });
  }
}
