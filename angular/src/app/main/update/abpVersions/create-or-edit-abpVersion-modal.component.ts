import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { AbpVersionsServiceProxy, CreateOrEditAbpVersionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditAbpVersionModal',
    templateUrl: './create-or-edit-abpVersion-modal.component.html',
})
export class CreateOrEditAbpVersionModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    abpVersion: CreateOrEditAbpVersionDto = new CreateOrEditAbpVersionDto();

    constructor(
        injector: Injector,
        private _abpVersionsServiceProxy: AbpVersionsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(abpVersionId?: number): void {
        if (!abpVersionId) {
            this.abpVersion = new CreateOrEditAbpVersionDto();
            this.abpVersion.id = abpVersionId;

            this.active = true;
            this.modal.show();
        } else {
            this._abpVersionsServiceProxy.getAbpVersionForEdit(abpVersionId).subscribe((result) => {
                this.abpVersion = result.abpVersion;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._abpVersionsServiceProxy
            .createOrEdit(this.abpVersion)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
