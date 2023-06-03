// @ts-check
// Note: type annotations allow type checking and IDEs autocompletion
require('dotenv').config()
const lightCodeTheme = require('prism-react-renderer/themes/github');
const darkCodeTheme = require('prism-react-renderer/themes/dracula');

/** @type {import('@docusaurus/types').Config} */
const config = {
  title: 'AWS Concepts',
  tagline: 'Delve into the world of AWS Cloud with our platform. We provide comprehensive coverage of the latest news, updates, and concepts in AWS Cloud, offering our readers a one-stop destination for everything AWS-related.',
  favicon: 'img/favicon.ico',

  url: process.env.REACT_APP_WEBSITE_URL,
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
          {to: '/blog', label: 'Blog', position: 'left'}
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
    plugins: [
      [
        'docusaurus-plugin-dotenv',
      {
          path: "./.env", 
          systemvars: true, 
      }
      ]
    ],
};

module.exports = config;
