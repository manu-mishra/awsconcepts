'use client'
import React, { useState } from 'react'

export const ViewToggle = () => {
    const [navOpen, setNavOpen] = useState(false);

const toggleNav = () => {
    setNavOpen(!navOpen);
  };
  return (
    <button className="hamburger" onClick={toggleNav}>☰</button>
  )
}
