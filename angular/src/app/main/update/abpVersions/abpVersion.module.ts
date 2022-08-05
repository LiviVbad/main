import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AbpVersionRoutingModule } from './abpVersion-routing.module';
import { AbpVersionsComponent } from './abpVersions.component';
import { CreateOrEditAbpVersionModalComponent } from './create-or-edit-abpVersion-modal.component';

@NgModule({
    declarations: [AbpVersionsComponent, CreateOrEditAbpVersionModalComponent],
    imports: [AppSharedModule, AbpVersionRoutingModule, AdminSharedModule],
})
export class AbpVersionModule {}
