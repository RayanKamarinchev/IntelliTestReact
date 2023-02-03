import React from 'react';
import style from "./css/card.module.css"
import {wait} from "@testing-library/user-event/dist/utils";

function Blog() {
    let moreInfo = React.createRef();
    function onHover(){
        setTimeout(function(){
            moreInfo.current.style.display = "block";
        }, 150);
    }
    function onLeave(){
        moreInfo.current.style.display = "none";
    }
    let card = {
        subject: "Физика",
        grade: "10",
        heading: "Електромагнитни Вълни",
        description: "просто тест",
        school: "ППМГ Добри Чинтулов",
        num_questions: 20,
        average_score: 87.5,
        students: 15,
        time: 30
    }
    return (
        <div className="center">

            <div className={style.card} onMouseEnter={onHover} onMouseLeave={onLeave}>
                <div className={style.additional}>
                    <div className={style.userCard}>
                        <h1 className={style.level}>
                            {card.subject}
                        </h1>
                        <h1 className={style.points}>
                            {card.grade} клас
                        </h1>
                    </div>
                    <div className={style.more} ref={moreInfo}>
                        <h1>Jane Doe</h1>
                        <div className={style.coords}>
                            <span>Group Name</span>
                            <span>Joined January 2019</span>
                        </div>
                        <div className={style.coords}>
                            <span>Position/Role</span>
                            <span>City, Country</span>
                        </div>
                        <div className={style.stats}>
                            <div>
                                <div className={style.title}>Awards</div>
                                <i className="fa fa-trophy"></i>
                                <div className={style.value}>2</div>
                            </div>
                            <div>
                                <div className={style.title}>Matches</div>
                                <i className="fa fa-gamepad"></i>
                                <div className={style.value}>27</div>
                            </div>
                            <div>
                                <div className={style.title}>Pals</div>
                                <i className="fa fa-group"></i>
                                <div className={style.value}>123</div>
                            </div>
                            <div>
                                <div className={style.title}>Coffee</div>
                                <i className="fa fa-coffee"></i>
                                <div className={[style.infinity, style.value]}>∞</div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className={style.general}>
                    <h1>Jane Doe</h1>
                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce a volutpat mauris, at molestie
                        lacus. Nam vestibulum sodales odio ut pulvinar.</p>
                    <span className={style.more}>Mouse over the card for more info</span>
                </div>
            </div>
        </div>
    );
}

export default Blog;