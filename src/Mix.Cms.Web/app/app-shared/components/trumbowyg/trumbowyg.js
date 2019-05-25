modules.component('trumbowyg', {
  templateUrl: '/app/app-shared/components/trumbowyg/trumbowyg.html',
  bindings: {
    options: '<?',
    ngDisabled: '<?',
    placeholder: '@?',
    onFocus: '&?',
    onBlur: '&?',
    onInit: '&?',
    onChange: '&?',
    onResize: '&?',
    onPaste: '&?',
    onOpenfullscreen: '&?',
    onClosefullscreen: '&?',
    onClose: '&?',
    removeformatPasted: '=',
  },
  require: {
    ngModel: 'ngModel'
  },
  controller: [
    '$element',
    '$scope',
    '$attrs',
    'ngAppSettings',
    function ($element, $scope, $attrs) {
      var ctrl = this;
      const TBW_EVENTS = [
        'focus',
        'blur',
        'init',
        'change',
        'resize',
        'paste',
        'openfullscreen',
        'closefullscreen',
        'close'
      ],
        EVENTS_PREFIX = 'tbw';
      ctrl.editorConfigurations = {
        core: {},
        plugins: {
          removeformatPasted: true,
          imageWidthModalEdit: true,
          semantic: false,
          btnsDef: {
            // Customizables dropdowns
            image: {
              dropdown: ['insertImage', 'upload', 'noembed'],
              ico: 'insertImage'
            }
          },
          btns: [
            ['table'],
            ['emoji'],
            ['formatting'],
            ['strong', 'em', 'del', 'underline'],
            ['fontsize'],
            ['highlight'],
            ['link'],
            ['image'],
            ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
            ['unorderedList', 'orderedList'],
            ['foreColor', 'backColor'],
            ['preformatted'],
            ['horizontalRule'],
            ['fullscreen'],
            ['viewHTML']
          ],
          plugins: {
            // Add imagur parameters to upload plugin
            upload: {
              serverPath: 'https://api.imgur.com/3/image',
              fileFieldName: 'image',
              headers: {
                'Authorization': 'Client-ID 9e57cb1c4791cea'
              },
              urlPropertyName: 'data.link'
            },
            table: {},
            fontfamily: {
              init: function (trumbowyg) {
                trumbowyg.o.plugins.fontfamily = trumbowyg.o.plugins.fontfamily || defaultOptions;
                trumbowyg.addBtnDef('fontfamily', {
                  dropdown: buildDropdown(trumbowyg),
                  hasIcon: false,
                  text: trumbowyg.lang.fontFamily
                });
              }
            }
          }
        }
      };
      ctrl.getElementReference = () => angular.element($element.find('> div'));

      ctrl.getEditorReference = () => ctrl.getElementReference().find('.trumbowyg-editor');

      ctrl.updateModelValue = () => {
        $scope.$applyAsync(() => {
          const value = ctrl.getEditorReference().trumbowyg('html');
          ctrl.ngModel.$setViewValue(value)
        });
      }

      ctrl.emitEvent = (event) => {
        const attr = $attrs.$normalize(`on-${event}`);
        if (angular.isFunction(this[attr])) {
          $scope.$applyAsync(() => this[attr]());
        }
      }

      ctrl.initializeEditor = (element, options) => {
        if(ctrl.removeformatPasted){
          ctrl.editorConfigurations.plugins.removeformatPasted = ctrl.removeformatPasted == 'true';
        }
        element.trumbowyg(ctrl.editorConfigurations.plugins)
          .on('tbwchange', () => ctrl.updateModelValue())
          .on('tbwpaste', () => ctrl.updateModelValue());
        angular.forEach(TBW_EVENTS, (event) => {
          element.on(`${EVENTS_PREFIX}${event}`, () => ctrl.emitEvent(event));
        })
        ctrl.ngModel.$render();
      }

      ctrl.$onDestroy = () => {
        ctrl.getElementReference().trumbowyg('destroy');
      };

      ctrl.$onChanges = (changes) => {
        const element = ctrl.getElementReference();

        if (changes.options && !changes.options.isFirstChange()) {
          element.trumbowyg('destroy');
        }

        if (changes.options) {
          ctrl.initializeEditor(element, angular.extend({}, ctrl.options));
        }

        if (changes.ngDisabled) {
          element.trumbowyg(ctrl.ngDisabled ? 'disable' : 'enable');
        }

        if (changes.placeholder) {
          ctrl.getEditorReference().attr('placeholder', ctrl.placeholder);
        }
      };

      ctrl.$onInit = () => {
        ctrl.ngModel.$render = () => {
          const element = ctrl.getEditorReference();
          element.trumbowyg('html', ctrl.ngModel.$modelValue);
        };        
      };
    }
  ]
});
