import React from 'react';
import clsx from 'clsx';
import styles from './styles.module.css';

type FeatureItem = {
  title: string;
  imageUrl: string;
  description: JSX.Element;
};

const FeatureList: FeatureItem[] = [
  {
    title: 'AWS Cloud News',
    imageUrl: '/img/clipart-1.png',
    description: (
      <>
        Stay updated with the latest news and trends in AWS cloud. We provide a 
        curated selection of the most relevant news about AWS services and updates.
      </>
    ),
  },
  {
    title: 'Daily AWS Blogs Summary',
    imageUrl: '/img/clipart-4.png',
    description: (
      <>
        Never miss a blog post from AWS. We provide a daily summary of blog posts 
        so you can stay informed about the latest tips, tricks and guides.
      </>
    ),
  },
  {
    title: 'Common AWS Solutions',
    imageUrl: '/img/clipart-2.png',
    description: (
      <>
        Explore common solutions and architectures in AWS. This section can be your 
        guide to understanding and implementing common patterns and architectures in AWS.
      </>
    ),
  },
];

function Feature({title, imageUrl, description}: FeatureItem) {
  return (
    <div className={clsx('col col--4')}>
      <div className="text--center">
        <img src={imageUrl} className={styles.featureImage} alt={title} />
      </div>
      <div className="text--center padding-horiz--md">
        <h3>{title}</h3>
        <p>{description}</p>
      </div>
    </div>
  );
}

export default function HomepageFeatures(): JSX.Element {
  return (
    <section className={styles.features}>
      <div className="container">
        <div className="row">
          {FeatureList.map((props, idx) => (
            <Feature key={idx} {...props} />
          ))}
        </div>
      </div>
    </section>
  );
}
