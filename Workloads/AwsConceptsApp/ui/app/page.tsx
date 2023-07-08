import { headers } from 'next/headers'
import styles from './page.module.css';
export default function Home() {
  return (
    <>
  <main>
  <div className={styles.hero}>
      <h1 className={styles.hero__title}>AWS Concepts</h1>
      <p className={styles.hero__subtitle}>
        Delve into the world of AWS Cloud with our platform. We provide comprehensive coverage of the latest news, updates, and concepts in AWS Cloud, offering our readers a one-stop destination for everything AWS-related.
      </p>
    </div>
    <section className={styles.features}>
  <div className={styles.feature}>
    <img src="/img/clipart-1.png" alt="AWS Cloud News" />
    <h3>AWS Cloud News</h3>
    <p>Stay updated with the latest news and trends in AWS cloud. We provide a curated selection of the most relevant news about AWS services and updates.</p>
  </div>
  <div className={styles.feature}>
    <img src="/img/clipart-4.png" alt="Daily AWS Blogs Summary" />
    <h3>Daily AWS Blogs Summary</h3>
    <p>Never miss a blog post from AWS. We provide a daily summary of blog posts so you can stay informed about the latest tips, tricks and guides.</p>
  </div>
  <div className={styles.feature}>
    <img src="/img/clipart-2.png" alt="Common AWS Solutions" />
    <h3>Common AWS Solutions</h3>
    <p>Explore common solutions and architectures in AWS. This section can be your guide to understanding and implementing common patterns and architectures in AWS.</p>
  </div>
</section>

  </main>

  <footer>
    <p>Copyright &copy; 2023 Your Name</p>
  </footer>
    </>
  )
}
