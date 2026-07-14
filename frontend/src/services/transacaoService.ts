import api from './api';
import type { Transacao, CriarTransacaoPayload } from '../types/transacao';

export async function listarTransacoes(pessoaId?: number): Promise<Transacao[]> {
  const params = pessoaId !== undefined ? { pessoaId } : {};
  const { data } = await api.get<Transacao[]>('/transacoes', { params });
  return data;
}

export async function criarTransacao(payload: CriarTransacaoPayload): Promise<Transacao> {
  const { data } = await api.post<Transacao>('/transacoes', payload);
  return data;
}
