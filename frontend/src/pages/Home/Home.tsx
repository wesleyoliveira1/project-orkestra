import { Link } from 'react-router-dom';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Separator } from '@/components/ui/separator';

export default function Home() {
  return (
    <section>
      <div>
        <h1>Project Orkestra</h1>

        <p>Gestão Inteligente de Operações</p>
      </div>

      <Separator />

      <div>
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
    </section>
  );
}
