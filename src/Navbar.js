import React from 'react';
import {Link, useLocation} from "react-router-dom";

function Navbar() {
    const {pathname} = useLocation();
    console.log(pathname)
    return (
        <nav className="navbar">
            <div className="navbar-container container">
                <div className="hamburger-lines">
                    <span className="line line1"></span>
                    <span className="line line2"></span>
                    <span className="line line3"></span>
                </div>
                <ul className="menu-items">
                    <li><Link className={(pathname === '/') ? 'active' : ''} to="/">Home</Link></li>
                    <li><Link className={(pathname === '/blog') ? 'active' : ''} to="/blog">Тестове</Link></li>
                    <li><Link className={(pathname === '/homepage') ? 'active' : ''} to="/">Дневник</Link></li>
                    <li><Link className={(pathname === '/homepage') ? 'active' : ''} to="/">Контакти</Link></li>
                </ul>
                <h3 className="logo"><span className="logoCol1">Intelli</span><span className="logoCol2">Test</span></h3>
            </div>
        </nav>
    );
}

export default Navbar;