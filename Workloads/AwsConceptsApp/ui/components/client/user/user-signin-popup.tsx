'use client'
import { Authenticator } from '@aws-amplify/ui-react'
import React, { useEffect, useRef } from 'react'
import './user-persona.css';

interface Props {
  showPopup: boolean;
  setShowPopup: (show: boolean) => void;
}

const SignInPopup: React.FC<Props> = ({ showPopup, setShowPopup }) => {
  const popupRef:any = useRef(null);

  useEffect(() => {
    const handleClickOutside = (event:any) => {
      if (popupRef.current && !popupRef.current.contains(event.target)) {
        setShowPopup(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [setShowPopup]);

  return showPopup ? (
    <div className="popup" ref={popupRef}>
      <Authenticator loginMechanisms={['email']} signUpAttributes={['nickname']}></Authenticator>
    </div>
  ) : null;
};

export default SignInPopup;
