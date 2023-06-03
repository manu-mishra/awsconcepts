import { Auth } from "aws-amplify"
export const applicationConfig = {
    Auth: {
        userPoolId: (process.env.REACT_APP_WEBSITE_AUTH_AWS_USER_POOL_ID as string),
        userPoolWebClientId: process.env.REACT_APP_WEBSITE_AUTH_AWS_USER_POOL_CLIENT_ID,
        mandatorySignIn: false,
        oauth: {
            domain: process.env.REACT_APP_WEBSITE_AUTH_OAUTH_DOMAIN,
            scope: ['email', 'profile', 'openid'],
            redirectSignIn: process.env.REACT_APP_WEBSITE_URL,
            redirectSignOut: process.env.REACT_APP_WEBSITE_URL,
            responseType: 'code'
        }
    },
    API: {
        endpoints: [
            {
                name: "api",
                endpoint: process.env.REACT_APP_WEBSITE_API_URL,
                custom_header: async () => {
                    return { Authorization: `Bearer ${(await Auth.currentSession()).getIdToken().getJwtToken()}` }
                  }
            }
        ]
    }

}

