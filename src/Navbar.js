import React from 'react';
import {Link} from "react-router-dom";

function Navbar() {
    return (
        <nav className="navbar">
            <div className="navbar-container container">
                <div className="hamburger-lines">
                    <span className="line line1"></span>
                    <span className="line line2"></span>
                    <span className="line line3"></span>
                </div>
                <ul className="menu-items">
                    <li><Link to="/">Home</Link></li>
                    <li><Link to="/">Explore</Link></li>
                    <li><Link to="/">Dashboard</Link></li>
                    <li><Link to="/">Contact</Link></li>
                </ul>
                <h3 className="logo"><span className="logoCol1">Intelli</span><span className="logoCol2">Test</span></h3>
            </div>
        </nav>
    );
}

export default Navbar;