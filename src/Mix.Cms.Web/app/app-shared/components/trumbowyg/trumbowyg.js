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
      function ($element, $scope, $attrs,ngAppSettings) {
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
          element.trumbowyg(ngAppSettings.editorConfigurations.plugins)
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
