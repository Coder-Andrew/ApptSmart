import styles from '@/components/home/welcomebanner.module.css'
import Link from 'next/link';
import Image from "next/image"
import { IoIosStar } from "react-icons/io";
import ROUTES from '@/lib/routes';
import satisfiedUser from '@/public/Home/ActiveUsers.png';


const WelcomeBanner = () => {
    return (
      <div className="container">
        <div className={`bg-tertiary-radial ${styles.welcomeContainer}`}>
          <div className={styles.left}>
            <span className={`text-secondary ${styles.ratingBanner}`}><IoIosStar />Rated #1 for upcoming appointment booking software</span>
            <p className={styles.primaryHeader}>
              <span className={styles.lineBreak}>Professional</span>
              <span className={styles.lineBreak}>scheduling</span>
              <span>with a </span>
              <span className="text-secondary font-secondary">personal touch</span>
            </p>
            <p className={styles.secondaryHeader}>
              <span className={styles.lineBreak}>We&apos;re here to link you directly to a number of providers,</span>
              <span>effortlessly connecting you with the services you need.</span>
            </p>
            <div className={styles.buttonGroup}>
              <Link href={ROUTES.apptsmartSchedule} className={`button-primary`}>View Schedule</Link>
              <Link href={'/contact-us'} className={`button-secondary`}>Learn More &rarr;</Link>
            </div>
          </div>
          <div className={styles.right}>
            <div className={`bg-background ${styles.userCount}`}>
              <div className={styles.top}>
                <svg
                  viewBox="0 0 50 20"  
                  width={50}
                >
                  <circle cy={10} cx={10} r={10} fill="red" />
                  <text fill="black" x="6.5" y="12.5" fontSize={10}>H</text>
                  <circle cy={10} cx={20} r={10} fill="cyan" />
                  <text fill="black" x="16.5" y="12.5" fontSize={10}>A</text>
                  <circle cy={10} cx={30} r={10} fill="lightgreen" />
                  <text fill="black" x="26.5" y="12.5" fontSize={10}>J</text>
                  <circle cy={10} cx={40} r={10} fill="purple" />
                  <text fill="black" x="36.5" y="12.5" fontSize={10}>K</text>
                </svg>
                <span className={`text-secondary`}>4,550+</span>
              </div>
              <div className={`text-muted ${styles.bottom}`}>Satisfied Users</div>
            </div>
            <Image
              src={satisfiedUser}
              height={674} //{674}
              width={762} //{762}
              alt="Satisfied user"
            ></Image>
          </div>
        </div>
      </div>
    );
}
 
export default WelcomeBanner;