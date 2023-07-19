/** @type {import('next').NextConfig} */
const nextConfig = {
    output: 'standalone',
    compiler: {
        removeConsole: {
            exclude: ['error'],
          },
      },
}

module.exports = nextConfig
