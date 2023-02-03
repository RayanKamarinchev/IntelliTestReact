import React from 'react';
import "./css/footer.css"
import "./index.css"
import { Link } from 'react-router-dom';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import { faFacebook, faGithub } from "@fortawesome/free-brands-svg-icons"
function Footer() {
    return (
        <div className="footer-clean">
            <footer>
                <div className="container">
                    <div>
                        <div className="col-lg-3 item social">
                            <a href="https://www.facebook.com/profile.php?id=100077942102799">
                                <FontAwesomeIcon icon={faFacebook}/></a>
                            <a href="https://github.com/RayanKamarinchev"><FontAwesomeIcon icon={faGithub}/></a>
                            <p><br/>Email: <Link href="mailto:rayan2008@gmail.com">rayan2008bg@gmail.com</Link></p>
                            <hr/>
                            <p>Всички права запазени. Строго забранено е копиране, изтегляне и публикуване на
                                каквито и да е материали от уебсайта, без предварително писмено съгласие от
                                автора.</p>
                            <p className="copyright">© Copyright 2022</p>
                        </div>
                    </div>
                </div>
            </footer>
        </div>
    );
}

export default Footer;