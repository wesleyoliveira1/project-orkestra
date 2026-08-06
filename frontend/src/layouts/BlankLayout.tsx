import { Outlet } from 'react-router-dom';

export default function BlankLayout() {
  return (
    <div>
      <div>
        <Outlet />
      </div>
    </div>
  );
}
