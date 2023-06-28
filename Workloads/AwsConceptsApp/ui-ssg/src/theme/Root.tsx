import { Amplify } from 'aws-amplify';
import React from 'react';
import { applicationConfig } from '../configuration/amplify/amplify-config';
import { Authenticator } from '@aws-amplify/ui-react';
import '@aws-amplify/ui-react/styles.css';
try {
  Amplify.configure(applicationConfig);
} catch (error) {
  console.log(error);
}

// Default implementation, that you can customize
function Root({children}) {
  return <>
  <Authenticator.Provider>
  {children}
  </Authenticator.Provider>
    
  </>;
}

export default Root;