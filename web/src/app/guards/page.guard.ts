import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';

export const pageGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const isAuthenticated = authService.isAuthenticated();
  const isLoginPage = state.url === '/auth/login';

  if (isAuthenticated && isLoginPage) {
    router.navigate(['/timelog/form']);
    return false;
  }

  if (!isAuthenticated && !isLoginPage) {
    router.navigate(['/auth/login']);
    return false;
  }

  return true;
};
