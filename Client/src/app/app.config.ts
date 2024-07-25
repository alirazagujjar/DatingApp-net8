import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import{provideHttpClient, withInterceptors} from '@angular/common/http';
import { routes } from './app.routes';
import{provideAnimations} from '@angular/platform-browser/animations'
import { provideToastr } from 'ngx-toastr';
import { errorInterceptor } from './_intercptors/error.interceptor';
export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([errorInterceptor])),
    provideAnimations(),
    provideToastr({
      positionClass:'toast-bottom-right'
    })
  ]
  
};
