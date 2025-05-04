import styles from "@/components/home/infopanel.module.css"
import Image from "next/image";
import womanHoldingPhone from '@/public/Home/WomanHoldingPhone.jpg';
import womanVideoConference from '@/public/Home/WomanInVideoConference.png';

import { IoCalendar } from "react-icons/io5";
import { MdOutlineContactMail } from "react-icons/md";
import { FaFolder } from "react-icons/fa";
import { BsShieldFillCheck } from "react-icons/bs";


const InfoPanel = () => {
    const iconSize = 25;
    // TODO: Fix the image sizing of these items and images
    const panelInfos = [
        {
            'title': 'Hassle-Free Appointment Scheduling',
            'info': [
                'Book your appointments with ease',
                'through our simple online platform, allowing you to schedule at',
                'your convenience, anytime, from anywhere.'
            ],
            'image': {
                'src': womanHoldingPhone,
                'alt': 'A woman holding her phone'
            },
            'icon': <IoCalendar className={`text-secondary ${styles.gridIcon}`} size={iconSize}/>
        },
        {
            'title': 'Virtual Consultations',
            'info': [
                'Access safe and convenient virtual consultations from',
                'home. Our encrypted video calls ensure privacy and',
                'eliminate the need for in-person visits.'
            ],            
            'image': {
                'src': womanVideoConference,
                'alt': 'A woman in a video call on her phone with a provider'
            },
            'icon': <MdOutlineContactMail className={`text-secondary ${styles.gridIcon}`} size={iconSize}/>
        },
        {
            'title': 'Records Management',
            'info': [
                'Effortlessly manage and access your records with a secure',
                'and organized system that keeps all your important',
                'information in one place, ensuring quick and easy retrieval whenever you need it.'
            ],
            'icon': <FaFolder className={`text-secondary ${styles.gridIcon}`} size={iconSize}/>
        },
        {
            'title': 'Effortless Appointment Tracking',
            'info': [
                'Enjoy immediate confirmation of your appointments along',
                'with timely notifications, so you\'re always prepared and',
                'never miss a scheduled meeting.'
            ],
            'icon': <BsShieldFillCheck className={`text-secondary ${styles.gridIcon}`} size={iconSize}/>
        },
    ]

    return (
        <>
            <div className={`bg-secondary ${styles.container}`}>
                <div className={`container`}>
                    <p className={`text-background ${styles.containerTitle}`}>
                        See What We've Got for
                        <span className={`text-tertiary font-secondary`}> You</span>
                    </p>
                    <div className={styles.grid}>
                        { panelInfos.map((panelInfo, index) => (
                            <div key={index} className={`bg-background ${styles.gridPanel}`}>
                                <div className={panelInfo.image ? styles.flexWrapper : ''}>
                                    <span className={`bg-secondary-background ${styles.iconWrapper}`}>
                                        {panelInfo.icon}
                                    </span>
                                    <p className={`text-primary ${styles.gridTitle}`}>
                                        {panelInfo.title}
                                    </p>
                                    <p className={`text-muted ${styles.gridInfo}`}>
                                        {panelInfo.info.map((infoTitle, infoTitleIndex) => (
                                            <span key={infoTitleIndex} className="line-break">
                                                {infoTitle}
                                            </span>
                                        ))}
                                    </p>
                                </div>
                                { panelInfo.image && (
                                    <div className={styles.imageWrapper}>
{/* LEFT OFF TRYING TO FIGURE OUT HOW TO SCALE THE IMAGES PROPERLY, YOU MIGHT NEED TO POTENTIALLY INTRODUCE ANOTHER DIV THAT HANDLES SIZING, OR A FLEX LAYOUT */}
                                        <Image
                                            placeholder="blur"
                                            src={panelInfo.image.src}
                                            alt={panelInfo.image.alt}
                                            fill
                                            style={{objectFit: 'contain' }}
                                        />
                                    </div>
                                )
                            }
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </>
    );
}
 
export default InfoPanel;