import React from 'react';
import ColorModeToggle from '@theme-original/ColorModeToggle';
import UserPersona from '@site/src/components/User/user-persona';

export default function ColorModeToggleWrapper(props) {
  return (
    <>
      <UserPersona></UserPersona>
      <ColorModeToggle {...props} />
    </>
  );
}
