'use client'
import { useAuthenticator } from '@aws-amplify/ui-react'
import { populateUserProfile } from '../../../model/user-access';
import { Auth } from 'aws-amplify';
import { Authenticator } from '@aws-amplify/ui-react';
import React, { useEffect, useState } from 'react'
import SignInPopup from './user-signin-popup';
import SignOutPopup from './user-signout-popup';

const UserPersona = () => {
  const [showSignInPopup, setShowSignInPopup] = useState(false);
  const [showSignOutPopup, setShowSignOutPopup] = useState(false);
  const { authStatus, signOut, user } = useAuthenticator((context) => [context.authStatus, context.signOut, context.user]);
  const [userSignedInStatus, setUserSignedInStatus] = useState<boolean>(false);

  useEffect(() => {
    if (authStatus === 'authenticated') {
      Auth.currentSession().then(() => {
        let userProfile = populateUserProfile(user);
        localStorage["userProfile"] = JSON.stringify(userProfile);
        setUserSignedInStatus(true);
        setShowSignInPopup(false);
      });
    } else {
      localStorage.removeItem("userProfile");
      setUserSignedInStatus(false);
    }
  }, [authStatus, user, signOut]);

  const handleButtonClick = () => {
    console.log(userSignedInStatus);
    if (userSignedInStatus) {
      Auth.signOut();
      //setShowSignInPopup(!showSignInPopup);
      //setShowSignOutPopup(!showSignOutPopup);
    } else {
      setShowSignInPopup(!showSignInPopup);
    }
  }

  return (
    <>
      <Authenticator.Provider>
        <button onClick={handleButtonClick} className="button-secondary">{userSignedInStatus ? "Hello! " + JSON.parse(localStorage["userProfile"])?.nickName : "Sign in"}</button>
        <SignInPopup showPopup={showSignInPopup} setShowPopup={setShowSignInPopup} />
        <SignOutPopup showPopup={showSignOutPopup} setShowPopup={setShowSignOutPopup} signOut={Auth.signOut} />
      </Authenticator.Provider>
    </>
  )
}

export default UserPersona;
