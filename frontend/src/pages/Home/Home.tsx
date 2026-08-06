import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Separator } from '@/components/ui/separator';

export default function Home() {
  return (
    <main className="space-y-6 p-8">
      <div>
        <h1 className="text-2x1 font-bold">Project Orkestra</h1>

        <p className="text-muted-foreground">Gestão Inteligente de Operações</p>
      </div>

      <Separator />

      <div className="grid gap-4 md:grid-cols-3">
        <Card>
          <CardHeader>
            <CardTitle> Funcionários </CardTitle>
          </CardHeader>

          <CardContent>Gestão de Colaboradores</CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle> Escalas </CardTitle>
          </CardHeader>

          <CardContent>Controle de Jornadas</CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle> Relatórios </CardTitle>
          </CardHeader>

          <CardContent>Indicadores de Negócios</CardContent>
        </Card>
      </div>
      <Button>Começar</Button>
    </main>
  );
}
