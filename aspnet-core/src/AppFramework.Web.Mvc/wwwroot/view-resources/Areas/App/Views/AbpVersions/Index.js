﻿(function () {
    $(function () {

        var _$abpVersionsTable = $('#AbpVersionsTable');
        var _abpVersionsService = abp.services.app.abpVersions;
		var _entityTypeFullName = 'AppFramework.Update.AbpVersion';
        
       var $selectedDate = {
            startDate: null,
            endDate: null,
        }

        $('.date-picker').on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format('MM/DD/YYYY'));
        });

        $('.startDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        }, (date) => {
            $selectedDate.startDate = date;
        }).on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        }, (date) => {
            $selectedDate.endDate = date;
        }).on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.AbpVersions.Create'),
            edit: abp.auth.hasPermission('Pages.AbpVersions.Edit'),
            'delete': abp.auth.hasPermission('Pages.AbpVersions.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/AbpVersions/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AbpVersions/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditAbpVersionModal'
                });
                   


		        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
		        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        var getDateFilter = function (element) {
            if ($selectedDate.startDate == null) {
                return null;
            }
            return $selectedDate.startDate.format("YYYY-MM-DDT00:00:00Z"); 
        }
        
        var getMaxDateFilter = function (element) {
            if ($selectedDate.endDate == null) {
                return null;
            }
            return $selectedDate.endDate.format("YYYY-MM-DDT23:59:59Z"); 
        }

        var dataTable = _$abpVersionsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _abpVersionsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#AbpVersionsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					versionFilter: $('#VersionFilterId').val(),
					downloadUrlFilter: $('#DownloadUrlFilterId').val(),
					changelogURLFilter: $('#ChangelogURLFilterId').val(),
					isEnableFilter: $('#IsEnableFilterId').val(),
					isForcedFilter: $('#IsForcedFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    },
                    targets: 0
                },
                {
                    width: 120,
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.abpVersion.id });                                
                            }
                        },
                        {
                            text: app.localize('History'),
                            iconStyle: 'fas fa-history mr-2',
                            visible: function () {
                                return entityHistoryIsEnabled();
                            },
                            action: function (data) {
                                _entityTypeHistoryModal.open({
                                    entityTypeFullName: _entityTypeFullName,
                                    entityId: data.record.abpVersion.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteAbpVersion(data.record.abpVersion);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "abpVersion.name",
						 name: "name"   
					},
					{
						targets: 3,
						 data: "abpVersion.version",
						 name: "version"   
					},
					{
						targets: 4,
						 data: "abpVersion.downloadUrl",
						 name: "downloadUrl"   
					},
					{
						targets: 5,
						 data: "abpVersion.changelogURL",
						 name: "changelogURL"   
					},
					{
						targets: 6,
						 data: "abpVersion.isEnable",
						 name: "isEnable"  ,
						render: function (isEnable) {
							if (isEnable) {
								return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 7,
						 data: "abpVersion.isForced",
						 name: "isForced"  ,
						render: function (isForced) {
							if (isForced) {
								return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					}
            ]
        });

        function getAbpVersions() {
            dataTable.ajax.reload();
        }

        function deleteAbpVersion(abpVersion) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _abpVersionsService.delete({
                            id: abpVersion.id
                        }).done(function () {
                            getAbpVersions(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

		$('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewAbpVersionButton').click(function () {
            _createOrEditModal.open();
        });        

		

        abp.event.on('app.createOrEditAbpVersionModalSaved', function () {
            getAbpVersions();
        });

		$('#GetAbpVersionsButton').click(function (e) {
            e.preventDefault();
            getAbpVersions();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getAbpVersions();
		  }
		});
		
		
		
    });
})();
