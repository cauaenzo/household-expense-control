import { useEffect, useState } from 'react';
import type { Transacao, CriarTransacaoPayload, TipoTransacao } from '../types/transacao';
import type { Pessoa } from '../types/pessoa';
import { listarTransacoes, criarTransacao } from '../services/transacaoService';
import { listarPessoas } from '../services/pessoaService';
import { extrairMensagemErro } from '../hooks/useErroApi';
import MensagemErro from '../components/MensagemErro';
import styles from './Transacoes.module.css';

const formatarMoeda = (v: number) =>
  v.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });

export default function Transacoes() {
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [filtroPessoaId, setFiltroPessoaId] = useState<string>('');
  const [descricao, setDescricao] = useState('');
  const [valor, setValor] = useState('');
  const [tipo, setTipo] = useState<TipoTransacao>('Despesa');
  const [pessoaId, setPessoaId] = useState<string>('');
  const [erro, setErro] = useState<string | null>(null);
  const [carregando, setCarregando] = useState(false);

  async function carregarPessoas() {
    try {
      const lista = await listarPessoas();
      setPessoas(lista);
    } catch (e) {
      setErro(extrairMensagemErro(e));
    }
  }

  async function carregarTransacoes(pessoaFiltro?: number) {
    try {
      const lista = await listarTransacoes(pessoaFiltro);
      setTransacoes(lista);
    } catch (e) {
      setErro(extrairMensagemErro(e));
    }
  }

  useEffect(() => {
    carregarPessoas();
    carregarTransacoes();
  }, []);

  function handleFiltroChange(e: React.ChangeEvent<HTMLSelectElement>) {
    const val = e.target.value;
    setFiltroPessoaId(val);
    carregarTransacoes(val ? parseInt(val, 10) : undefined);
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setErro(null);

    const valorNum = parseFloat(valor.replace(',', '.'));
    if (!descricao.trim()) {
      setErro('A descricao e obrigatoria.');
      return;
    }
    if (isNaN(valorNum) || valorNum <= 0) {
      setErro('O valor deve ser um numero maior que zero.');
      return;
    }
    if (!pessoaId) {
      setErro('Selecione uma pessoa.');
      return;
    }

    const payload: CriarTransacaoPayload = {
      descricao: descricao.trim(),
      valor: valorNum,
      tipo,
      pessoaId: parseInt(pessoaId, 10),
    };

    setCarregando(true);
    try {
      await criarTransacao(payload);
      setDescricao('');
      setValor('');
      setTipo('Despesa');
      setPessoaId('');
      await carregarTransacoes(filtroPessoaId ? parseInt(filtroPessoaId, 10) : undefined);
    } catch (e) {
      setErro(extrairMensagemErro(e));
    } finally {
      setCarregando(false);
    }
  }

  return (
    <div>
      <h1 className={styles.titulo}>Transacoes</h1>

      <div className={styles.secao}>
        <p className={styles.secaoTitulo}>Registrar transacao</p>
        <MensagemErro mensagem={erro} />
        <form className={styles.form} onSubmit={handleSubmit}>
          <div className={styles.campo}>
            <label htmlFor="descricao">Descricao</label>
            <input
              id="descricao"
              type="text"
              value={descricao}
              onChange={(e) => setDescricao(e.target.value)}
              placeholder="Descricao"
              style={{ width: 240 }}
            />
          </div>
          <div className={styles.campo}>
            <label htmlFor="valor">Valor (R$)</label>
            <input
              id="valor"
              type="text"
              inputMode="decimal"
              value={valor}
              onChange={(e) => setValor(e.target.value)}
              placeholder="0,00"
              style={{ width: 110 }}
            />
          </div>
          <div className={styles.campo}>
            <label htmlFor="tipo">Tipo</label>
            <select
              id="tipo"
              value={tipo}
              onChange={(e) => setTipo(e.target.value as TipoTransacao)}
              style={{ width: 120 }}
            >
              <option value="Despesa">Despesa</option>
              <option value="Receita">Receita</option>
            </select>
          </div>
          <div className={styles.campo}>
            <label htmlFor="pessoa">Pessoa</label>
            <select
              id="pessoa"
              value={pessoaId}
              onChange={(e) => setPessoaId(e.target.value)}
              style={{ width: 180 }}
            >
              <option value="">Selecione...</option>
              {pessoas.map((p) => (
                <option key={p.id} value={p.id}>
                  {p.nome} ({p.idade} anos)
                </option>
              ))}
            </select>
          </div>
          <button type="submit" className={styles.botaoSalvar} disabled={carregando}>
            {carregando ? 'Salvando...' : 'Registrar'}
          </button>
        </form>
      </div>

      <div className={styles.secao}>
        <p className={styles.secaoTitulo}>Lancamentos</p>
        <div className={styles.filtroPessoa}>
          <label htmlFor="filtro">Filtrar por pessoa:</label>
          <select id="filtro" value={filtroPessoaId} onChange={handleFiltroChange}>
            <option value="">Todas</option>
            {pessoas.map((p) => (
              <option key={p.id} value={p.id}>
                {p.nome}
              </option>
            ))}
          </select>
        </div>
        {transacoes.length === 0 ? (
          <p className={styles.semDados}>Nenhuma transacao encontrada.</p>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Descricao</th>
                <th>Tipo</th>
                <th>Valor</th>
                <th>Pessoa</th>
              </tr>
            </thead>
            <tbody>
              {transacoes.map((t) => (
                <tr key={t.id}>
                  <td>{t.descricao}</td>
                  <td>
                    <span className={t.tipo === 'Receita' ? styles.tipoReceita : styles.tipoDespesa}>
                      {t.tipo}
                    </span>
                  </td>
                  <td>{formatarMoeda(t.valor)}</td>
                  <td>{t.nomePessoa}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}
