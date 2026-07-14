import { useEffect, useState } from 'react';
import type { RelatorioTotais } from '../types/relatorio';
import { obterTotais } from '../services/relatorioService';
import { extrairMensagemErro } from '../hooks/useErroApi';
import MensagemErro from '../components/MensagemErro';
import styles from './Totais.module.css';

const formatarMoeda = (v: number) =>
  v.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });

export default function Totais() {
  const [relatorio, setRelatorio] = useState<RelatorioTotais | null>(null);
  const [erro, setErro] = useState<string | null>(null);
  const [carregando, setCarregando] = useState(false);

  async function carregar() {
    setCarregando(true);
    setErro(null);
    try {
      const dados = await obterTotais();
      setRelatorio(dados);
    } catch (e) {
      setErro(extrairMensagemErro(e));
    } finally {
      setCarregando(false);
    }
  }

  useEffect(() => {
    carregar();
  }, []);

  return (
    <div>
      <h1 className={styles.titulo}>Totais por pessoa</h1>
      <MensagemErro mensagem={erro} />

      <button className={styles.botaoAtualizar} onClick={carregar} disabled={carregando}>
        {carregando ? 'Atualizando...' : 'Atualizar'}
      </button>

      {relatorio && (
        <>
          <div className={styles.secao}>
            <p className={styles.secaoTitulo}>Por pessoa</p>
            {relatorio.pessoas.length === 0 ? (
              <p className={styles.semDados}>Nenhuma pessoa cadastrada.</p>
            ) : (
              <table>
                <thead>
                  <tr>
                    <th>Nome</th>
                    <th>Receitas</th>
                    <th>Despesas</th>
                    <th>Saldo</th>
                  </tr>
                </thead>
                <tbody>
                  {relatorio.pessoas.map((p) => (
                    <tr key={p.id}>
                      <td>{p.nome}</td>
                      <td className={styles.receita}>{formatarMoeda(p.totalReceitas)}</td>
                      <td className={styles.despesa}>{formatarMoeda(p.totalDespesas)}</td>
                      <td className={p.saldo >= 0 ? styles.saldoPositivo : styles.saldoNegativo}>
                        {formatarMoeda(p.saldo)}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </div>

          <div className={styles.totalGeral}>
            <p className={styles.totalGeralTitulo}>Total geral</p>
            <div className={styles.totalGeralGrid}>
              <div className={styles.totalItem}>
                <span className={styles.totalLabel}>Receitas</span>
                <span className={`${styles.totalValor} ${styles.receita}`}>
                  {formatarMoeda(relatorio.totalGeral.totalReceitas)}
                </span>
              </div>
              <div className={styles.totalItem}>
                <span className={styles.totalLabel}>Despesas</span>
                <span className={`${styles.totalValor} ${styles.despesa}`}>
                  {formatarMoeda(relatorio.totalGeral.totalDespesas)}
                </span>
              </div>
              <div className={styles.totalItem}>
                <span className={styles.totalLabel}>Saldo liquido</span>
                <span
                  className={`${styles.totalValor} ${
                    relatorio.totalGeral.saldoLiquido >= 0
                      ? styles.saldoPositivo
                      : styles.saldoNegativo
                  }`}
                >
                  {formatarMoeda(relatorio.totalGeral.saldoLiquido)}
                </span>
              </div>
            </div>
          </div>
        </>
      )}
    </div>
  );
}
