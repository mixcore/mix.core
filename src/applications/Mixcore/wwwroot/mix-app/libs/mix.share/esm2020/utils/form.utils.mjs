export class FormUtils {
    static validateForm(form) {
        Object.values(form.controls).forEach((control) => {
            if (control.invalid) {
                control.markAsDirty();
                control.markAsTouched();
                control.updateValueAndValidity({ onlySelf: true });
            }
        });
        return form.valid;
    }
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZm9ybS51dGlscy5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvdXRpbHMvZm9ybS51dGlscy50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFFQSxNQUFNLE9BQU8sU0FBUztJQUNiLE1BQU0sQ0FBQyxZQUFZLENBQUMsSUFBZTtRQUN4QyxNQUFNLENBQUMsTUFBTSxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxPQUF3QixFQUFFLEVBQUU7WUFDaEUsSUFBSSxPQUFPLENBQUMsT0FBTyxFQUFFO2dCQUNuQixPQUFPLENBQUMsV0FBVyxFQUFFLENBQUM7Z0JBQ3RCLE9BQU8sQ0FBQyxhQUFhLEVBQUUsQ0FBQztnQkFDeEIsT0FBTyxDQUFDLHNCQUFzQixDQUFDLEVBQUUsUUFBUSxFQUFFLElBQUksRUFBRSxDQUFDLENBQUM7YUFDcEQ7UUFDSCxDQUFDLENBQUMsQ0FBQztRQUVILE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQztJQUNwQixDQUFDO0NBQ0YiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBYnN0cmFjdENvbnRyb2wsIEZvcm1Hcm91cCB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcclxuXHJcbmV4cG9ydCBjbGFzcyBGb3JtVXRpbHMge1xyXG4gIHB1YmxpYyBzdGF0aWMgdmFsaWRhdGVGb3JtKGZvcm06IEZvcm1Hcm91cCk6IGJvb2xlYW4ge1xyXG4gICAgT2JqZWN0LnZhbHVlcyhmb3JtLmNvbnRyb2xzKS5mb3JFYWNoKChjb250cm9sOiBBYnN0cmFjdENvbnRyb2wpID0+IHtcclxuICAgICAgaWYgKGNvbnRyb2wuaW52YWxpZCkge1xyXG4gICAgICAgIGNvbnRyb2wubWFya0FzRGlydHkoKTtcclxuICAgICAgICBjb250cm9sLm1hcmtBc1RvdWNoZWQoKTtcclxuICAgICAgICBjb250cm9sLnVwZGF0ZVZhbHVlQW5kVmFsaWRpdHkoeyBvbmx5U2VsZjogdHJ1ZSB9KTtcclxuICAgICAgfVxyXG4gICAgfSk7XHJcblxyXG4gICAgcmV0dXJuIGZvcm0udmFsaWQ7XHJcbiAgfVxyXG59XHJcbiJdfQ==