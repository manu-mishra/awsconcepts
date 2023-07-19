import React from 'react';
import AppLogo from '../header/AppLogo'
import {AppHeaderRight} from '../header/AppHeaderRight'
import ErrorBoundary from '../../client/ErrorBoundary';

export const AppHeader = () => {
  return (
    <header>

      <div className="header-left">
        <AppLogo></AppLogo>
      </div>

      {/* <nav id="nav-menu" className={`nav ${navOpen ? 'nav-open' : ''}`}>
        <ul>
          <li><a href="#home">Home</a></li>
          <li><a href="#about">About</a></li>
        </ul>
      </nav> */}

      <div className="header-right">
        <ErrorBoundary>
        <AppHeaderRight></AppHeaderRight>
        </ErrorBoundary>
      </div>
    </header>
  )
}