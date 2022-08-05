import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AbpVersionsComponent } from './abpVersions.component';

const routes: Routes = [
    {
        path: '',
        component: AbpVersionsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AbpVersionRoutingModule {}
