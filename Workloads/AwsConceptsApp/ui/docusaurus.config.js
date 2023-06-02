// @ts-check
// Note: type annotations allow type checking and IDEs autocompletion

const lightCodeTheme = require('prism-react-renderer/themes/github');
const darkCodeTheme = require('prism-react-renderer/themes/dracula');

/** @type {import('@docusaurus/types').Config} */
const config = {
  title: 'AWS Concepts',
  tagline: 'Exploring AWS Cloud Concepts and News',
  favicon: 'img/favicon.ico',

  url: 'https://dev.awsconcepts.com',
  baseUrl: '/',

  organizationName: 'manu-mishra', // Your GitHub username
  projectName: 'awsconcepts', // Your repo name

  onBrokenLinks: 'throw',
  onBrokenMarkdownLinks: 'warn',

  i18n: {
    defaultLocale: 'en',
    locales: ['en'],
  },

  presets: [
    [
      'classic',
      /** @type {import('@docusaurus/preset-classic').Options} */
      ({
        docs: {
          sidebarPath: require.resolve('./sidebars.js'),
          editUrl:
            'https://github.com/manu-mishra/awsconcepts/tree/main/Workloads/AwsConceptsApp/ui/docs', // Your GitHub repo link
        },
        blog: {
          showReadingTime: true,
          editUrl:
            'https://github.com/manu-mishra/awsconcepts/tree/main/Workloads/AwsConceptsApp/ui/blog', // Your GitHub repo link
        },
        theme: {
          customCss: require.resolve('./src/css/custom.css'),
        },
      }),
    ],
  ],

  themeConfig:
    /** @type {import('@docusaurus/preset-classic').ThemeConfig} */
    ({
      image: 'img/AwsConcepts-social-card.png', // Your social card
      navbar: {
        title: 'AWS Concepts',
        logo: {
          alt: 'AWS Concepts Logo',
          src: 'img/awsclogo-192x192.png', // Your logo
        },
        items: [
          {
            type: 'docSidebar',
            sidebarId: 'newsSidebar',
            position: 'left',
            label: 'Aws News',
          },
          {to: '/blog', label: 'Blog', position: 'right'},
          {
            href: 'https://github.com/manu-mishra/awsconcepts',
            label: 'GitHub',
            position: 'right',
          },
        ],
      },
      footer: {
        style: 'dark',
        links: [
          {
            title: 'Docs',
            items: [
              {
                label: 'News',
                to: '/docs/intro',
              },
            ],
          },
          {
            title: 'More',
            items: [
              {
                label: 'Blog',
                to: '/blog',
              },
              {
                label: 'GitHub',
                href: 'https://github.com/manu-mishra/awsconcepts',
              },
            ],
          },
        ],
        copyright: `Copyright © ${new Date().getFullYear()} AWS Concepts.`,
      },
      prism: {
        theme: lightCodeTheme,
        darkTheme: darkCodeTheme,
      },
    }),
};

module.exports = config;
