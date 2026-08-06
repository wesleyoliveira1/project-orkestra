import { Outlet } from 'react-router-dom';

export default function AuthLayout() {
  return (
    <div>
      <div>
        <h1>Login</h1>
        <div>
          <Outlet />
        </div>
      </div>
    </div>
  );
}
