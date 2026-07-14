import styles from './MensagemErro.module.css';

interface Props {
  mensagem: string | null;
}

export default function MensagemErro({ mensagem }: Props) {
  if (!mensagem) return null;
  return <div className={styles.erro}>{mensagem}</div>;
}
