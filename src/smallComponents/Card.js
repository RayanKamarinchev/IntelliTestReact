import React from 'react';
import style from "./../css/card.module.css"

function Card({test}) {
    let moreInfo = React.createRef();
    let timeout;
    function onHover(){
        timeout = setTimeout(function(){
            moreInfo.current.style.display = "block";
        }, 290);
    }
    function onLeave(){
        window.clearTimeout(timeout)
        moreInfo.current.style.display = "none";
    }
    return (
        <div className={style.card} onMouseEnter={onHover} onMouseLeave={onLeave}>
            <div className={style.additional} style={{background: `linear-gradient(${test.color1} , ${test.color2})`}}>
                <div className={style.userCard}>
                    <h1 className={style.level}>
                        {test.subject}
                    </h1>
                    <h1 className={style.points}>
                        {test.grade} клас
                    </h1>
                </div>
                <div className={style.more} ref={moreInfo}>
                    <h1>{test.heading}</h1>
                    <div className={style.coords}>
                        <span>Училище: {test.school}</span>
                    </div>
                    <div className={style.stats}>
                        <div>
                            <div className={style.title}>Въпроси</div>
                            <div className={style.value}>{test.num_questions}</div>
                        </div>
                        <div>
                            <div className={style.title}>Време</div>
                            <div className={style.value}>{test.time}<span> m</span></div>
                        </div>
                        <div>
                            <div className={style.title}>Ср. Рез.</div>
                            <div className={style.value}>{test.average_score}%</div>
                        </div>
                        <div>
                            <div className={style.title}>Изпитници</div>
                            <div className={[style.value]}>{test.students}</div>
                        </div>
                    </div>
                </div>
            </div>
            <div className={style.general}>
                <h1>{test.heading}</h1>
                <p>{test.description}</p>
            </div>
        </div>
    );
}

export default Card;