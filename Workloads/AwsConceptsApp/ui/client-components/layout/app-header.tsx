'use client'
import React, { useState } from 'react';
import Image from 'next/image'

export const AppHeader = () => {
  const [navOpen, setNavOpen] = useState(false);

  const toggleNav = () => {
    setNavOpen(!navOpen);
  };
  return (
    <header>

      <div className="header-left">
        <button className="hamburger" onClick={toggleNav}>☰</button>
        <Image
              src="/AwsConceptLogoRaw-nbg.png"
              alt="Aws concepts Logo"
              className="dark:invert"
              width={50}
              height={50}
              priority
            />
        <span>Aws Concepts</span>
      </div>

      <nav id="nav-menu" className={`nav ${navOpen ? 'nav-open' : ''}`}>
        <ul>
          <li><a href="#home">Home</a></li>
          <li><a href="#about">About</a></li>
        </ul>
      </nav>

      <div className="header-right">
        <button>Sign In</button>
        <button>Sign Out</button>
        <img src="path-to-user-persona.png" alt="User Persona" />
      </div>
    </header>
  )
}

