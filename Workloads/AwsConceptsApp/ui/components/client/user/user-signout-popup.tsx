'use client'
import { Button } from '@aws-amplify/ui-react'
import React, { useEffect, useRef } from 'react'
import './user-persona.css';

interface Props {
  showPopup: boolean;
  setShowPopup: (show: boolean) => void;
  signOut: () => void;
}

const SignOutPopup: React.FC<Props> = ({ showPopup, setShowPopup, signOut }) => {
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
      <Button onClick={() => { signOut(); setShowPopup(false); }}>Sign Out</Button>
    </div>
  ) : null;
};

export default SignOutPopup;
