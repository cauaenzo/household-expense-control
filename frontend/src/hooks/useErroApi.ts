import axios from 'axios';

export function extrairMensagemErro(erro: unknown): string {
  if (axios.isAxiosError(erro) && erro.response?.data?.mensagem) {
    return erro.response.data.mensagem as string;
  }
  if (erro instanceof Error) {
    return erro.message;
  }
  return 'Ocorreu um erro inesperado.';
}
