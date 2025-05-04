import Link from "next/link";
import styles from "@/app/page.module.css"
import WelcomeBanner from "@/components/home/WelcomeBanner";
import InfoPanel from "@/components/home/InfoPanel";


export default function Home() {

  return (
    <>
      <WelcomeBanner />
      <InfoPanel />
    </>
  );
}
