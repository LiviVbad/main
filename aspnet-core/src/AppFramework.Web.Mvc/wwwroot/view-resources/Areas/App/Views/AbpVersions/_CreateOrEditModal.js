(function ($) {
    app.modals.CreateOrEditAbpVersionModal = function () {

        var _abpVersionsService = abp.services.app.abpVersions;

        var _modalManager;
        var _$abpVersionInformationForm = null;

		
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$abpVersionInformationForm = _modalManager.getModal().find('form[name=AbpVersionInformationsForm]');
            _$abpVersionInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$abpVersionInformationForm.valid()) {
                return;
            }

            

            var abpVersion = _$abpVersionInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _abpVersionsService.createOrEdit(
				abpVersion
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditAbpVersionModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);