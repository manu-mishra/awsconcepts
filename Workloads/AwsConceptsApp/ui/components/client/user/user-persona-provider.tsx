'use client'
import { Authenticator } from '@aws-amplify/ui-react';
import React from 'react'
import UserPersona from './user-persona';
import { applicationConfig } from '../../../configuration/amplify/amplify-config';
import '@aws-amplify/ui-react/styles.css';
import { Amplify } from 'aws-amplify';

try {
  Amplify.configure(applicationConfig);
} catch (error) {
  console.log(error);
}
const UserPersonaProvider = () => {

  return (
    <>
      <Authenticator.Provider>
        <UserPersona></UserPersona>
      </Authenticator.Provider>
    </>
  )
}

export default UserPersonaProvider;
