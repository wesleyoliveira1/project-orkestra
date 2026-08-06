import { BrowserRouter, Routes, Route } from 'react-router-dom';
import AppLayout from '../layouts/AppLayout';
import AuthLayout from '../layouts/AuthLayout';
import BlankLayout from '../layouts/BlankLayout';
import HomePage from '../pages/Home/Home';
import NotFoundPage from '@/pages/Notfound/Notfound';
import LoginPage from '@/pages/Login/login';

export default function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<AppLayout />}>
          <Route index element={<HomePage />} />
        </Route>

        <Route path="/login" element={<AuthLayout />}>
          <Route index element={<LoginPage />} />
        </Route>

        <Route path="*" element={<BlankLayout />}>
          <Route index element={<NotFoundPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
