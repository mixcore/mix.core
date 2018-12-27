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
      this.editorConfigurations = {
        core: {},
        plugins: {
          removeformatPasted: true,
          imageWidthModalEdit: true,
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
      this.getElementReference = () => angular.element($element.find('> div'));

      this.getEditorReference = () => this.getElementReference().find('.trumbowyg-editor');

      this.updateModelValue = () => {
        $scope.$applyAsync(() => {
          const value = this.getEditorReference().trumbowyg('html');
          this.ngModel.$setViewValue(value)
        });
      }

      this.emitEvent = (event) => {
        const attr = $attrs.$normalize(`on-${event}`);
        if (angular.isFunction(this[attr])) {
          $scope.$applyAsync(() => this[attr]());
        }
      }

      this.initializeEditor = (element, options) => {
        element.trumbowyg(this.editorConfigurations.plugins)
          .on('tbwchange', () => this.updateModelValue())
          .on('tbwpaste', () => this.updateModelValue());
        angular.forEach(TBW_EVENTS, (event) => {
          element.on(`${EVENTS_PREFIX}${event}`, () => this.emitEvent(event));
        })
        this.ngModel.$render();
      }

      this.$onDestroy = () => {
        this.getElementReference().trumbowyg('destroy');
      };

      this.$onChanges = (changes) => {
        const element = this.getElementReference();

        if (changes.options && !changes.options.isFirstChange()) {
          element.trumbowyg('destroy');
        }

        if (changes.options) {
          this.initializeEditor(element, angular.extend({}, this.options));
        }

        if (changes.ngDisabled) {
          element.trumbowyg(this.ngDisabled ? 'disable' : 'enable');
        }

        if (changes.placeholder) {
          this.getEditorReference().attr('placeholder', this.placeholder);
        }
      };

      this.$onInit = () => {
        this.ngModel.$render = () => {
          const element = this.getEditorReference();
          element.trumbowyg('html', this.ngModel.$modelValue);
        };
      };
    }
  ]
});
