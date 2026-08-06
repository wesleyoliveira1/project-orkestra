export default function Sidebar() {
  return (
    <aside className="w-64 border-r min-h-screen p-4">
      <h2 className="txt-x1 font-bold">Orkestra</h2>

      <nav className="mt-6 space-y-2">
        <div>Dashboard</div>
        <div>Funcionários</div>
        <div>Escalas</div>
        <div>Relatórios</div>
      </nav>
    </aside>
  );
}
