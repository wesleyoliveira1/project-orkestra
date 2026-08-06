import { Outlet } from 'react-router-dom';

export default function AppLayout() {
  return (
    <div>
      <header>
        <div>
          <h1>Project Orkestra</h1>
        </div>
      </header>
      <main>
        <Outlet />
      </main>
    </div>
  );
}
