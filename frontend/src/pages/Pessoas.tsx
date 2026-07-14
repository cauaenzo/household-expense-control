import { useEffect, useState } from 'react';
import type { Pessoa, CriarPessoaPayload } from '../types/pessoa';
import { listarPessoas, criarPessoa, excluirPessoa } from '../services/pessoaService';
import { extrairMensagemErro } from '../hooks/useErroApi';
import MensagemErro from '../components/MensagemErro';
import styles from './Pessoas.module.css';

export default function Pessoas() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [nome, setNome] = useState('');
  const [idade, setIdade] = useState('');
  const [erro, setErro] = useState<string | null>(null);
  const [carregando, setCarregando] = useState(false);

  async function carregar() {
    try {
      const lista = await listarPessoas();
      setPessoas(lista);
    } catch (e) {
      setErro(extrairMensagemErro(e));
    }
  }

  useEffect(() => {
    carregar();
  }, []);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setErro(null);

    const idadeNum = parseInt(idade, 10);
    if (!nome.trim()) {
      setErro('O nome e obrigatorio.');
      return;
    }
    if (isNaN(idadeNum) || idadeNum < 0) {
      setErro('A idade deve ser um numero valido nao negativo.');
      return;
    }

    const payload: CriarPessoaPayload = { nome: nome.trim(), idade: idadeNum };
    setCarregando(true);
    try {
      await criarPessoa(payload);
      setNome('');
      setIdade('');
      await carregar();
    } catch (e) {
      setErro(extrairMensagemErro(e));
    } finally {
      setCarregando(false);
    }
  }

  async function handleExcluir(id: number, nomePessoa: string) {
    if (!confirm(`Excluir "${nomePessoa}" e todas as suas transacoes?`)) return;
    setErro(null);
    try {
      await excluirPessoa(id);
      await carregar();
    } catch (e) {
      setErro(extrairMensagemErro(e));
    }
  }

  return (
    <div>
      <h1 className={styles.titulo}>Pessoas</h1>

      <div className={styles.secao}>
        <p className={styles.secaoTitulo}>Cadastrar pessoa</p>
        <MensagemErro mensagem={erro} />
        <form className={styles.form} onSubmit={handleSubmit}>
          <div className={styles.campo}>
            <label htmlFor="nome">Nome</label>
            <input
              id="nome"
              type="text"
              value={nome}
              onChange={(e) => setNome(e.target.value)}
              placeholder="Nome completo"
            />
          </div>
          <div className={styles.campo}>
            <label htmlFor="idade">Idade</label>
            <input
              id="idade"
              type="number"
              min={0}
              value={idade}
              onChange={(e) => setIdade(e.target.value)}
              placeholder="Anos"
              style={{ width: 100 }}
            />
          </div>
          <button type="submit" className={styles.botaoSalvar} disabled={carregando}>
            {carregando ? 'Salvando...' : 'Cadastrar'}
          </button>
        </form>
      </div>

      <div className={styles.secao}>
        <p className={styles.secaoTitulo}>Lista de pessoas</p>
        {pessoas.length === 0 ? (
          <p className={styles.semDados}>Nenhuma pessoa cadastrada.</p>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Nome</th>
                <th>Idade</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {pessoas.map((p) => (
                <tr key={p.id}>
                  <td>{p.nome}</td>
                  <td>{p.idade}</td>
                  <td>
                    <button
                      className={styles.botaoExcluir}
                      onClick={() => handleExcluir(p.id, p.nome)}
                    >
                      Excluir
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}
