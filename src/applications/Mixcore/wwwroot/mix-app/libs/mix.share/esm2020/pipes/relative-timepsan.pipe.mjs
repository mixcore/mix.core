import { Pipe } from '@angular/core';
import * as i0 from "@angular/core";
export class RelativeTimeSpanPipe {
    transform(value) {
        if (value == null || value === '' || value === 'N/A' || value !== value)
            return value;
        const current = new Date(new Date().toUTCString().substr(0, 25));
        const previous = new Date(value);
        const msPerMinute = 60 * 1000;
        const msPerHour = msPerMinute * 60;
        const elapsed = current.getTime() - previous.getTime();
        if (elapsed < msPerMinute) {
            return Math.round(elapsed / 1000) + ' seconds ago';
        }
        else if (elapsed < msPerHour) {
            return Math.round(elapsed / msPerMinute) + ' minutes ago';
        }
        else {
            return value;
        }
    }
}
RelativeTimeSpanPipe.ɵfac = function RelativeTimeSpanPipe_Factory(t) { return new (t || RelativeTimeSpanPipe)(); };
RelativeTimeSpanPipe.ɵpipe = /*@__PURE__*/ i0.ɵɵdefinePipe({ name: "relativeTimeSpan", type: RelativeTimeSpanPipe, pure: true, standalone: true });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && i0.ɵsetClassMetadata(RelativeTimeSpanPipe, [{
        type: Pipe,
        args: [{ name: 'relativeTimeSpan', standalone: true }]
    }], null, null); })();
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmVsYXRpdmUtdGltZXBzYW4ucGlwZS5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uLy4uL21peC5wb3J0YWwuYW5ndWxhci9saWJzL21peC5zaGFyZS9zcmMvcGlwZXMvcmVsYXRpdmUtdGltZXBzYW4ucGlwZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEVBQUUsSUFBSSxFQUFpQixNQUFNLGVBQWUsQ0FBQzs7QUFHcEQsTUFBTSxPQUFPLG9CQUFvQjtJQUN4QixTQUFTLENBQUMsS0FBb0I7UUFDbkMsSUFBSSxLQUFLLElBQUksSUFBSSxJQUFJLEtBQUssS0FBSyxFQUFFLElBQUksS0FBSyxLQUFLLEtBQUssSUFBSSxLQUFLLEtBQUssS0FBSztZQUFFLE9BQU8sS0FBSyxDQUFDO1FBRXRGLE1BQU0sT0FBTyxHQUFTLElBQUksSUFBSSxDQUFDLElBQUksSUFBSSxFQUFFLENBQUMsV0FBVyxFQUFFLENBQUMsTUFBTSxDQUFDLENBQUMsRUFBRSxFQUFFLENBQUMsQ0FBQyxDQUFDO1FBQ3ZFLE1BQU0sUUFBUSxHQUFTLElBQUksSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO1FBQ3ZDLE1BQU0sV0FBVyxHQUFXLEVBQUUsR0FBRyxJQUFJLENBQUM7UUFDdEMsTUFBTSxTQUFTLEdBQVcsV0FBVyxHQUFHLEVBQUUsQ0FBQztRQUMzQyxNQUFNLE9BQU8sR0FBVyxPQUFPLENBQUMsT0FBTyxFQUFFLEdBQUcsUUFBUSxDQUFDLE9BQU8sRUFBRSxDQUFDO1FBQy9ELElBQUksT0FBTyxHQUFHLFdBQVcsRUFBRTtZQUN6QixPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxHQUFHLElBQUksQ0FBQyxHQUFHLGNBQWMsQ0FBQztTQUNwRDthQUFNLElBQUksT0FBTyxHQUFHLFNBQVMsRUFBRTtZQUM5QixPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxHQUFHLFdBQVcsQ0FBQyxHQUFHLGNBQWMsQ0FBQztTQUMzRDthQUFNO1lBQ0wsT0FBTyxLQUFLLENBQUM7U0FDZDtJQUNILENBQUM7O3dGQWhCVSxvQkFBb0I7NkZBQXBCLG9CQUFvQjt1RkFBcEIsb0JBQW9CO2NBRGhDLElBQUk7ZUFBQyxFQUFFLElBQUksRUFBRSxrQkFBa0IsRUFBRSxVQUFVLEVBQUUsSUFBSSxFQUFFIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgUGlwZSwgUGlwZVRyYW5zZm9ybSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5cclxuQFBpcGUoeyBuYW1lOiAncmVsYXRpdmVUaW1lU3BhbicsIHN0YW5kYWxvbmU6IHRydWUgfSlcclxuZXhwb3J0IGNsYXNzIFJlbGF0aXZlVGltZVNwYW5QaXBlIGltcGxlbWVudHMgUGlwZVRyYW5zZm9ybSB7XHJcbiAgcHVibGljIHRyYW5zZm9ybSh2YWx1ZTogc3RyaW5nIHwgbnVsbCkge1xyXG4gICAgaWYgKHZhbHVlID09IG51bGwgfHwgdmFsdWUgPT09ICcnIHx8IHZhbHVlID09PSAnTi9BJyB8fCB2YWx1ZSAhPT0gdmFsdWUpIHJldHVybiB2YWx1ZTtcclxuXHJcbiAgICBjb25zdCBjdXJyZW50OiBEYXRlID0gbmV3IERhdGUobmV3IERhdGUoKS50b1VUQ1N0cmluZygpLnN1YnN0cigwLCAyNSkpO1xyXG4gICAgY29uc3QgcHJldmlvdXM6IERhdGUgPSBuZXcgRGF0ZSh2YWx1ZSk7XHJcbiAgICBjb25zdCBtc1Blck1pbnV0ZTogbnVtYmVyID0gNjAgKiAxMDAwO1xyXG4gICAgY29uc3QgbXNQZXJIb3VyOiBudW1iZXIgPSBtc1Blck1pbnV0ZSAqIDYwO1xyXG4gICAgY29uc3QgZWxhcHNlZDogbnVtYmVyID0gY3VycmVudC5nZXRUaW1lKCkgLSBwcmV2aW91cy5nZXRUaW1lKCk7XHJcbiAgICBpZiAoZWxhcHNlZCA8IG1zUGVyTWludXRlKSB7XHJcbiAgICAgIHJldHVybiBNYXRoLnJvdW5kKGVsYXBzZWQgLyAxMDAwKSArICcgc2Vjb25kcyBhZ28nO1xyXG4gICAgfSBlbHNlIGlmIChlbGFwc2VkIDwgbXNQZXJIb3VyKSB7XHJcbiAgICAgIHJldHVybiBNYXRoLnJvdW5kKGVsYXBzZWQgLyBtc1Blck1pbnV0ZSkgKyAnIG1pbnV0ZXMgYWdvJztcclxuICAgIH0gZWxzZSB7XHJcbiAgICAgIHJldHVybiB2YWx1ZTtcclxuICAgIH1cclxuICB9XHJcbn1cclxuIl19