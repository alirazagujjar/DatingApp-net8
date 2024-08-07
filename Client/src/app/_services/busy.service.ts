import { inject, Injectable } from '@angular/core';
import { NgxSpinnerService } from "ngx-spinner";

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestcount = 0;
  private spinnerService = inject(NgxSpinnerService);
  busy() {
    this.busyRequestcount++;
    this.spinnerService.show(undefined, {
      type: 'square-loader',
      bdColor: 'rgba(255,255,255,0)',
      color: '#333333'
    })
  }
  idle() {
    this.busyRequestcount--;
    console.log(this.busyRequestcount);
    if (this.busyRequestcount <= 0) {
      this.busyRequestcount = 0;
      this.spinnerService.hide();
    }
  }
}
