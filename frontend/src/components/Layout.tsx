import { NavLink, Outlet } from 'react-router-dom';
import styles from './Layout.module.css';

export default function Layout() {
  return (
    <>
      <header className={styles.header}>
        <div className={styles.headerInner}>
          <span className={styles.headerTitle}>Controle de Gastos</span>
          <nav className={styles.nav}>
            <NavLink
              to="/pessoas"
              className={({ isActive }) =>
                `${styles.navLink} ${isActive ? styles.navLinkActive : ''}`
              }
            >
              Pessoas
            </NavLink>
            <NavLink
              to="/transacoes"
              className={({ isActive }) =>
                `${styles.navLink} ${isActive ? styles.navLinkActive : ''}`
              }
            >
              Transacoes
            </NavLink>
            <NavLink
              to="/totais"
              className={({ isActive }) =>
                `${styles.navLink} ${isActive ? styles.navLinkActive : ''}`
              }
            >
              Totais
            </NavLink>
          </nav>
        </div>
      </header>
      <main className={styles.main}>
        <Outlet />
      </main>
    </>
  );
}
