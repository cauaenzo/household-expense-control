import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Layout from './components/Layout';
import Pessoas from './pages/Pessoas';
import Transacoes from './pages/Transacoes';
import Totais from './pages/Totais';

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<Navigate to="/pessoas" replace />} />
          <Route path="pessoas" element={<Pessoas />} />
          <Route path="transacoes" element={<Transacoes />} />
          <Route path="totais" element={<Totais />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
