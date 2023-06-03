import { useAuthenticator } from '@aws-amplify/ui-react'
import { populateUserProfile } from '@site/src/model/user-access';
import { Auth } from 'aws-amplify';
import React, { useEffect, useState } from 'react'
import SignInPopup from './user-signin-popup';
import SignOutPopup from './user-signout-popup';
import Link from '@docusaurus/Link';

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
    if(userSignedInStatus){
      setShowSignOutPopup(!showSignOutPopup);
    } else {
      setShowSignInPopup(!showSignInPopup);
    }
  }

  return (
    <>
    <Link
    onClick={handleButtonClick}
            className="button button--primary">
             {userSignedInStatus ? "Hello! " + JSON.parse(localStorage["userProfile"])?.nickName : "Sign in"}
          </Link>
      <SignInPopup showPopup={showSignInPopup} setShowPopup={setShowSignInPopup} />
      <SignOutPopup showPopup={showSignOutPopup} setShowPopup={setShowSignOutPopup} signOut={signOut} /> 
    </>
  )
}

export default UserPersona;
